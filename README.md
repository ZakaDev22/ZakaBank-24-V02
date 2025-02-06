## 📌 Project Overview
The **Bank Management System (BMS)** is a desktop application designed for efficient banking operations. It manages **clients, transactions, and transfers** while maintaining a clean three-tier architecture:
- **Data Access Layer (DAL):** Handles database interactions via **ADO.NET**.
- **Business Logic Layer (BLL):** Implements business rules and data validations.
- **User Interface (UI):** Built using **WinForms** with the **Guna UI Framework** for an enhanced user experience.

---
## Database Resources
- **Database Backup:** Download the complete database backup file [ZakaBank.bak](./My%20Database/ZakaBank.bak).
- **Database Schema Screenshot:**  
  ![Database Schema](./My%20Database/Database%20Diagram%20V.01.png)
---

## 🛠️ Technologies Used
- **C# WinForms** (with Guna UI Framework)
- **SQL Server** & **T-SQL**
- **ADO.NET** for database connectivity
- **System.Security.Cryptography** for secure password handling
- **Async/Await** for responsive operations
- **Git/GitHub** for version control
- **Data Visualization** using charts (Line, Pie, Bar, Doughnut)

---
## Permissions and Data Visualization

### Security & Permissions
The Bank Management System (BMS) uses a robust permissions system based on role-based access control (RBAC):

- **Binary Permissions:**  
  User roles are managed using a binary bitwise approach. Each permission is represented as a bit in a single integer, allowing for efficient storage and quick permission checks.  
  - For example, if a role has a permissions value of `13` (binary `1101`), this means the role has access to the first, third, and fourth permission flags.
  
- **Password Security:**  
  User passwords are secured using `System.Security.Cryptography` for hashing. This ensures that even if the database is compromised, the actual passwords remain protected.

- **Role-Based Access:**  
  Each user is assigned a role, and their permissions are determined by the integer value stored in the database. This system allows for flexible permission management, making it easy to add, remove, or modify access rights as needed.

---

### Data Visualization with Charts
To provide a clear view of banking activities, BMS includes interactive charts that visualize key metrics:

- **Line Chart:**  
  Displays the total transactions for the last 5 days, helping track daily performance trends.

- **Bar Chart:**  
  Illustrates the total transfers made in the last 5 days. This chart provides insights into the frequency and volume of transfers.

- **Pie Chart:**  
  Breaks down the transactions by type, including deposits, withdrawals, refunds, and payments. This helps in understanding the composition of banking operations.

- **Doughnut Chart:**  
  Visualizes client balance ranges. It segments clients into various balance groups to identify patterns and financial health across the client base.

These charts are dynamically generated and updated, providing real-time insights into the performance and usage of the system. The combination of role-based security and data visualization ensures that administrators and users can both manage access effectively and gain valuable insights into the system’s operations.

---

*Feel free to reach out or contribute if you have any suggestions or improvements regarding the permissions system or chart functionalities.*

---

## Stored Procedurs Key Features :

### 1️⃣ Transaction Handling: `sp_Transactions_AddNewTransaction`
This stored procedure manages multiple transaction types (Deposit, Withdrawal, Payment, Refund) in a single operation:
- **Deposit:** Increases the client's balance.
- **Withdrawal:** Decreases the balance if sufficient funds exist.
- **Payment:** Deducts the amount similar to a withdrawal.
- **Refund:** Adds the amount back to the balance.

**Stored Procedure Code:**

```sql
USE [ZakaBank]
GO

ALTER PROCEDURE [dbo].[sp_Transactions_AddNewTransaction]
    @ClientID INT,
    @Amount DECIMAL(18, 2),
    @TransactionTypeID INT,  -- Transaction type (1 = Deposit, 2 = Withdrawal, etc.)
    @Description VARCHAR(255),
    @AddedByUser INT,
    @TransactionID INT OUT
AS
BEGIN
    -- Declare a variable to hold the client's current balance
    DECLARE @CurrentBalance DECIMAL(18, 2);
    
    -- Get the current balance of the client
    SELECT @CurrentBalance = Balance
    FROM Client
    WHERE ClientID = @ClientID;

    -- Handle transaction logic based on TransactionTypeID
    IF @TransactionTypeID = 1  -- Deposit
    BEGIN
        -- Add the amount to the client's balance
        UPDATE Client
        SET Balance = @CurrentBalance + @Amount
        WHERE ClientID = @ClientID;
    END
    ELSE IF @TransactionTypeID = 2  -- Withdrawal
    BEGIN
        -- Check if the client has sufficient balance
        IF @CurrentBalance >= @Amount
        BEGIN
            -- Deduct the amount from the client's balance
            UPDATE Client
            SET Balance = @CurrentBalance - @Amount
            WHERE ClientID = @ClientID;
        END
        ELSE
        BEGIN
            -- Insufficient funds error
            RAISERROR('Insufficient funds for withdrawal', 16, 1);
            RETURN;
        END
    END
    ELSE IF @TransactionTypeID = 4  -- Payment
    BEGIN
        -- Deduct amount for specific service/payment
        UPDATE Client
        SET Balance = @CurrentBalance - @Amount
        WHERE ClientID = @ClientID;
    END
    ELSE IF @TransactionTypeID = 5  -- Refund
    BEGIN
        -- Refund logic similar to deposit
        UPDATE Client
        SET Balance = @CurrentBalance + @Amount
        WHERE ClientID = @ClientID;
    END

    -- Insert transaction record
    INSERT INTO Transactions (ClientID, Amount, TransactionTypeID, Description, TransactionDate, AddedByUser)
    VALUES (@ClientID, @Amount, @TransactionTypeID, @Description, GETDATE(), @AddedByUser);

    SET @TransactionID = SCOPE_IDENTITY();
END;
```
# Money Transfer Procedure: sp_Client_MakeTransfer

## Overview
The `sp_Client_MakeTransfer` stored procedure is a critical component of the Bank Management System (BMS). It securely manages money transfers between clients, ensuring that funds are accurately deducted and credited, while maintaining data integrity through the use of transactions and robust error handling.

## Key Features
- **Secure Transfer:** Uses the `UPDLOCK` mechanism to ensure that the sender's account is locked during balance validation, preventing race conditions.
- **Validation:** Checks for sufficient funds in the sender's account and verifies that the receiver's account exists before proceeding.
- **Atomic Operations:** Updates both the sender's and receiver's balances within a transaction to guarantee consistency.
- **Robust Error Handling:** Utilizes a `TRY...CATCH` block to log errors into an `ErrorLog` table and roll back the transaction if any error occurs.

## Technologies Used
- **SQL Server & T-SQL:** Core database engine and language.
- **Transactions & Concurrency Control:** Implements transactions along with the `UPDLOCK` hint to handle concurrent updates.
- **Error Handling:** Incorporates `TRY...CATCH` blocks for reliable error management.

## Stored Procedure Code
Below is the complete T-SQL code for the `sp_Client_MakeTransfer` stored procedure:

```sql
USE [ZakaBank]
GO

ALTER PROCEDURE [dbo].[sp_Client_MakeTransfer]
    @SenderClientID INT,
    @ReceiverClientID INT,
    @Amount DECIMAL(18,2),
    @Description VARCHAR(255),
    @AddedByUserID INT,
    @TransferID INT OUT
AS
BEGIN
    BEGIN TRY
        -- Start the transaction
        BEGIN TRANSACTION;

        -- Check if sender has sufficient balance
        IF NOT EXISTS (
            SELECT 1
            FROM Client WITH (UPDLOCK)
            WHERE ClientID = @SenderClientID AND Balance >= @Amount
        )
        BEGIN
            RAISERROR ('Insufficient balance in sender account.', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END

        -- Check if the receiver exists
        IF NOT EXISTS (
            SELECT 1
            FROM Client 
            WHERE ClientID = @ReceiverClientID
        )
        BEGIN
            RAISERROR ('Receiver account does not exist.', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END

        -- Update sender balance
        UPDATE Client
        SET Balance = Balance - @Amount
        WHERE ClientID = @SenderClientID;

        -- Update receiver balance
        UPDATE Client
        SET Balance = Balance + @Amount
        WHERE ClientID = @ReceiverClientID;

        -- Insert the transfer record
        INSERT INTO Transfers (SenderClientID, ReceiverClientID, Amount, TransferDate, Description, AddedByUserID)
        VALUES (@SenderClientID, @ReceiverClientID, @Amount, GETDATE(), @Description, @AddedByUserID);

        SET @TransferID = SCOPE_IDENTITY();

        -- Commit the transaction
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        -- Log the error into ErrorLog table
        INSERT INTO ErrorLog (ErrorMessage, ErrorLine, ErrorSeverity, ErrorProcedure, ErrorState)
        VALUES (
            ERROR_MESSAGE(), 
            ERROR_LINE(), 
            ERROR_SEVERITY(), 
            ERROR_PROCEDURE(), 
            ERROR_STATE()
        );

        -- Roll back the transaction in case of error
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        -- Rethrow the error
        THROW;
    END CATCH
END;
```
