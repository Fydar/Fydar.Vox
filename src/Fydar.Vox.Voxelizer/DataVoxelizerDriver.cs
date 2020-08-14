namespace Fydar.Vox.Voxelizer
{
	public abstract class DataVoxelizerDriver
	{
		public abstract void Process<T>(ref T buffer) where T : struct, IDriverBuffer;
	}
}
