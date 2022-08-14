using ILikeDapper.Model.Interface;

namespace ILikeDapper.Model.Implementation
{
    public class SimpleParameter : IParameter
    {
        public List<IAttribute> Attributes { get; set ; } = new List<IAttribute>();
        public int? Id { get; set; }
        public IAttribute? Parent { get; set; }
    }
}
