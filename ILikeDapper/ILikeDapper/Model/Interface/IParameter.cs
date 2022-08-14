namespace ILikeDapper.Model.Interface
{
    public interface IParameter : IAttribute
    {
        public List<IAttribute> Attributes { get; set; }
    }
}
