using System;
using Template.Model.Base;

namespace Template.Model
{
    public class ValueModel : IViewModel, ICreateModel, IUpdateModel
    {
        public string Description { get; set; }
        public long Id { get; set; }
    }
}
