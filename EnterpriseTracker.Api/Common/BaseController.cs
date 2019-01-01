using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;

namespace EnterpriseTracker.Api.Controllers.Common
{
    public class BaseController : ApiController
    {
        protected HttpResponseMessage CreateResponse(HttpStatusCode statusCode, ModelStateDictionary modelState, object requestObject)
        {
            //var ex = new VvaletiException("Bad Request");
            //ex.Data["request"] = JsonConvert.SerializeObject(requestObject);
            //foreach (var model in ModelState.Where(x => x.Value.Errors.Count > 0))
            //{
            //    foreach (var error in model.Value.Errors)
            //    {
            //        ex.Data.Add(model.Key, error.ErrorMessage);
            //    }
            //}
            //ErrorSignal.FromCurrentContext().Raise(ex);
            return Request.CreateResponse(HttpStatusCode.BadRequest);
        }
    }
}