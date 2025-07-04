using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace SisNikosPizza.Utilidades
{
    public static class ImageResizer
    {
        /// <summary>
        /// Redimensiona una imagen a un ancho específico manteniendo la relación de aspecto
        /// </summary>
        /// <param name="inputStream">Stream de la imagen original</param>
        /// <param name="outputPath">Ruta donde se guardará la imagen redimensionada</param>
        /// <param name="targetWidth">Ancho objetivo en píxeles</param>
        public static async Task ResizeImageAsync(Stream inputStream, string outputPath, int targetWidth = 255)
        {
            using var image = await Image.LoadAsync(inputStream);
            
            // Calcular la altura manteniendo la relación de aspecto
            var aspectRatio = (double)image.Height / image.Width;
            var targetHeight = (int)(targetWidth * aspectRatio);
            
            // Redimensionar la imagen
            image.Mutate(x => x.Resize(targetWidth, targetHeight));
            
            // Guardar la imagen redimensionada
            await image.SaveAsync(outputPath);
        }

        /// <summary>
        /// Redimensiona una imagen desde un archivo y la guarda en otra ubicación
        /// </summary>
        /// <param name="inputPath">Ruta del archivo de imagen original</param>
        /// <param name="outputPath">Ruta donde se guardará la imagen redimensionada</param>
        /// <param name="targetWidth">Ancho objetivo en píxeles</param>
        public static async Task ResizeImageAsync(string inputPath, string outputPath, int targetWidth = 255)
        {
            using var image = await Image.LoadAsync(inputPath);
            
            // Calcular la altura manteniendo la relación de aspecto
            var aspectRatio = (double)image.Height / image.Width;
            var targetHeight = (int)(targetWidth * aspectRatio);
            
            // Redimensionar la imagen
            image.Mutate(x => x.Resize(targetWidth, targetHeight));
            
            // Guardar la imagen redimensionada
            await image.SaveAsync(outputPath);
        }
    }
}
