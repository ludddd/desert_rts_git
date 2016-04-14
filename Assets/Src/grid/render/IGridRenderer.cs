namespace grid.render
{
    public interface IGridRenderer
    {
        void DrawCell(int cellIdx);
        void Render();
    }
}
