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

using GeekQuiz.Models;
using GeekQuiz.Services;
using GeekQuiz.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace GeekQuiz.Controllers
{
    public class StatisticsController : ApiController
    {
        private TriviaContext db;
        private StatisticsService statisticsService;

        public StatisticsController()
        {
            this.db = new TriviaContext();
            this.statisticsService = new StatisticsService(db);
        }

        protected override void Dispose(bool disposing)
        {
            this.db.Dispose();
            base.Dispose(disposing);
        }

        // GET api/statistics
        [ResponseType(typeof(StatisticsViewModel))]
        public async Task<IHttpActionResult> Get()
        {
            StatisticsViewModel statistics =
                await this.statisticsService.GenerateStatistics();
            if (statistics == null)
            {
                return NotFound();
            }

            return Ok(statistics);
        }
    }
}
