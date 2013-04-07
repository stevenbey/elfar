﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Elfar.WebApi
{
    public class ErrorLogController : ApiController
    {
        public IEnumerable<WebApiErrorLog> Get()
        {
            try { return Execute(p => p.List()).Select(errorLog => (WebApiErrorLog)errorLog); }
            catch(Exception e) { throw new ErrorLogException(e); }
        }
        public WebApiErrorLog Get(Guid id)
        {
            ErrorLog errorLog;
            try { errorLog = Execute(p => p.Get(id)); }
            catch (Exception e) { throw new ErrorLogException(e); }
            if (errorLog == null) throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            return (WebApiErrorLog) errorLog;
        }

        T Execute<T>(Func<IErrorLogProvider, T> func)
        {
            var route = Configuration.Routes.FirstOrDefault(r => r.RouteTemplate == ErrorLogRoute.Url);
            var provider = route == null ? null : route.DataTokens["provider"] as IErrorLogProvider;
            return provider == null ? default(T) : func(provider);
        }
    }
}