namespace PowerCode.CodeGeneration.Core
{
    // ----------------------- -------------------------- ------------------- -----------------

    public interface IDeclarationBuilder
    {
        string Build();
        void AddBuilder(IDeclarationBuilder builder);
    }
}
