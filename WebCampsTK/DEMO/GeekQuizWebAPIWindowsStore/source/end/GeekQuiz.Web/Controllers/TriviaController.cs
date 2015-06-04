﻿// ----------------------------------------------------------------------------------
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

using GeekQuiz.Models;
using GeekQuiz.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace GeekQuiz.Controllers
{
    public class TriviaController : ApiController
    {
        private TriviaContext db;
        private QuestionsService questionsService;
        private AnswersService answersService;

        const string UserId = "test";

        public TriviaController()
        {
            this.db = new TriviaContext();
            this.questionsService = new QuestionsService(db);
            this.answersService = new AnswersService(db);
        }

        public async Task<TriviaQuestion> Get()
        {
            TriviaQuestion nextQuestion =
                await this.questionsService.NextQuestionAsync(UserId);

            if (nextQuestion == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return nextQuestion;
        }

        public async Task<HttpResponseMessage> Post(TriviaAnswer answer)
        {
            if (ModelState.IsValid)
            {
                answer.UserId = UserId;
                bool isCorrect =false;
                try
                {
                    isCorrect = await this.answersService.StoreAsync(answer);
                }
                catch (Exception e)
                {

                }
                

                return Request.CreateResponse(HttpStatusCode.Created, isCorrect);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        protected override void Dispose(bool disposing)
        {
            this.db.Dispose();
            base.Dispose(disposing);
        }
    }
}