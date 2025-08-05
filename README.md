```text
git lfs install
git clone https://huggingface.co/CompVis/stable-diffusion-v1-4 -b onnx
```

- Copy the folders with the ONNX files to the C# project folder `\ImageGeneration\ImageGeneration`. The folders to copy are: `unet`, `vae_decoder`, `text_encoder`, `safety_checker`.
- Run the project
- Open test.html
