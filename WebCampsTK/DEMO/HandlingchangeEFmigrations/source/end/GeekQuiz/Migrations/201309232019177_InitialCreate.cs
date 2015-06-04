// ----------------------------------------------------------------------------------
// Microsoft Developer & Platform Evangelism
// 
// Copyright (c) Microsoft Corporation. All rights reserved.
// 
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
// EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
// OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// ----------------------------------------------------------------------------------
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
// ----------------------------------------------------------------------------------

namespace GeekQuiz.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TriviaAnswers",
                c => new
                    {
                        QuestionId = c.Int(nullable: false),
                        OptionId = c.Int(nullable: false),
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TriviaOptions", t => new { t.QuestionId, t.OptionId }, cascadeDelete: true)
                .Index(t => new { t.QuestionId, t.OptionId });
            
            CreateTable(
                "dbo.TriviaOptions",
                c => new
                    {
                        QuestionId = c.Int(nullable: false),
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        IsCorrect = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.QuestionId, t.Id })
                .ForeignKey("dbo.TriviaQuestions", t => t.QuestionId, cascadeDelete: true)
                .Index(t => t.QuestionId);
            
            CreateTable(
                "dbo.TriviaQuestions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TriviaAnswers", new[] { "QuestionId", "OptionId" }, "dbo.TriviaOptions");
            DropForeignKey("dbo.TriviaOptions", "QuestionId", "dbo.TriviaQuestions");
            DropIndex("dbo.TriviaAnswers", new[] { "QuestionId", "OptionId" });
            DropIndex("dbo.TriviaOptions", new[] { "QuestionId" });
            DropTable("dbo.TriviaQuestions");
            DropTable("dbo.TriviaOptions");
            DropTable("dbo.TriviaAnswers");
        }
    }
}
