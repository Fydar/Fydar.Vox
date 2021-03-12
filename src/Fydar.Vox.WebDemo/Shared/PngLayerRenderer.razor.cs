using Microsoft.AspNetCore.Components;
using System.Numerics;
using System.Threading.Tasks;

namespace Fydar.Vox.WebDemo.Shared
{
	public partial class PngLayerRenderer : ComponentBase
	{
		[Parameter]
		public DemoModel Model { get; set; }

		[Parameter]
		public Vector3 Position { get; set; }

		[Parameter]
		public Vector2 Rotation { get; set; }

		[Parameter]
		public float Scale { get; set; }

		[Parameter]
		public int FaceSize { get; set; }

		protected override Task OnInitializedAsync()
		{
			return Task.CompletedTask;
		}
	}
}
