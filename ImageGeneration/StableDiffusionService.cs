using SixLabors.ImageSharp;
using StableDiffusion.ML.OnnxRuntime;
using System.Globalization;

public class StableDiffusionService
{
    private readonly StableDiffusionConfig _config;

    public StableDiffusionService(IConfiguration configuration)
    {
        var sdConfigSection = configuration.GetSection("StableDiffusionConfig");
        var configModelPath = sdConfigSection["ModelPath"];

        string modelPath = string.IsNullOrEmpty(configModelPath) ? 
            Path.Combine(Directory.GetCurrentDirectory(), "OnnxModels") : 
            configModelPath;

        _config = new StableDiffusionConfig
        {
            NumInferenceSteps = int.Parse(sdConfigSection["NumInferenceSteps"]!),
            GuidanceScale = double.Parse(sdConfigSection["GuidanceScale"]!, CultureInfo.InvariantCulture),
            ExecutionProviderTarget = Enum.Parse<StableDiffusionConfig.ExecutionProvider>(sdConfigSection["ExecutionProviderTarget"]!),
            TextEncoderOnnxPath = Path.Combine(modelPath, "text_encoder", "model.onnx"),
            UnetOnnxPath = Path.Combine(modelPath, "unet", "model.onnx"),
            VaeDecoderOnnxPath = Path.Combine(modelPath, "vae_decoder", "model.onnx"),
            SafetyModelPath = Path.Combine(modelPath, "safety_checker", "model.onnx"),
            TokenizerOnnxPath = Path.Combine(Directory.GetCurrentDirectory(),"..", "StableDiffusion.ML.OnnxRuntime", "cliptokenizer.onnx"),
            OrtExtensionsPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "StableDiffusion.ML.OnnxRuntime", "ortextensions.dll"),
        };
    }

    public Image GenerateImage(string prompt)
    {
        return UNet.Inference(prompt, _config);
    }
}
