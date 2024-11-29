using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App6.Infra.Data
{
    public static class ProductSql
    {
        public const string AddCmd = @"
            INSERT INTO [dbo].[Product] (
                [Title] 
                ,[Price]) 
            VALUES (
                @Title, @Price) 
            SELECT SCOPE_IDENTITY()";

        public const string AllCmd = @"
            SELECT 
                [Id] 
                ,[Title] 
                ,[Price] 
                ,[Created] 
                ,[Modified] 
            FROM 
                [dbo].[Product]";

        public const string DeleteCmd = @"
            DELETE FROM [dbo].[Product] 
            WHERE [Id] = @Id";

        public const string FindCmd = @"
            SELECT 
                [Id] 
                ,[Title] 
                ,[Price] 
                ,[Created] 
                ,[Modified] 
            FROM [dbo].[Product] 
            WHERE [Id] = @Id";
    }
}
