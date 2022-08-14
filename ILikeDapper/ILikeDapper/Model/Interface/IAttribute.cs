namespace ILikeDapper.Model.Interface
{
    public interface IAttribute
    {
        public int? Id { get; set; }
        public IAttribute? Parent { get; set; }
    }
}
