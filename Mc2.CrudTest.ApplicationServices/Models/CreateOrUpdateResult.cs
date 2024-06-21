using System;
using System.Collections.Generic;
using System.Linq;

namespace Mc2.CrudTest.Domain2.Models
{
    public class CreateOrUpdateResult<TModel>
    {
        public CreateOrUpdateResult(){ }

        public CreateOrUpdateResult(TModel model)
        {
            Model = model;
            Errors = new List<int>();
        }

        public static CreateOrUpdateResult<TModel> AddErrors(IEnumerable<int> errors)
        {
            CreateOrUpdateResult<TModel> result = new()
            {
                Errors = errors
            };

            return result;
        }

        public static CreateOrUpdateResult<TModel> SetModel(TModel model)
        {
            CreateOrUpdateResult<TModel> result = new(model);

            return result;
        }

        /// <summary>
        /// The model to return
        /// </summary>
        public TModel Model { get; set; }

        /// <summary>
        /// The list of codes of the happened errors during the process
        /// </summary>
        public IEnumerable<int> Errors { get; set; }

        /// <summary>
        /// You will get TRUE, if there is no Error
        /// </summary>
        public bool IsOk => (Errors?.Count() ?? 0) == 0;

        public bool HasError => ! IsOk;
    }
}
