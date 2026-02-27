# IconConverter 2024

A robust C# WPF Application to convert image files to .ico and create multi-sized icons.

### How to use:

1.  **Open or Drag & Drop**: You can drag and drop files directly into the window or click `File -> Open` to load an image.
2.  **Select Region (Optional)**: To crop, click and drag on the image to select a region. Release to finish selection.
3.  **Choose Dimensions**: Go to the `Export` menu and check the desired icon dimensions (256x256, 128x128, etc.).
4.  **Save**:
    *   **Crop and Save**: Click `Export -> Crop and Save` to save the selected region as icons.
    *   **Save Whole Image**: Click `Export -> Save whole image as icon` to convert the entire image.
5.  **Merge Icons**: To merge multiple .ico files into a single multi-size icon file, click the `Merge` menu, choose the preferred option, and select the files to merge.

**Output:** Converted icons are saved in a folder named after the source image, located in the same directory as the source image.

<p align="center">
<img align="center" src="http://i.imgur.com/yCbhyqR.png" width="551"/>

### Key Features:

*   **Modern & Responsive**: Uses asynchronous processing so the UI never freezes, even with large files.
*   **High Quality**: Powered by **Magick.NET** (ImageMagick) for superior image resizing and format conversion.
*   **Robust**: comprehensive error handling ensures the app doesn't crash on file errors.
*   **Clean UI**: Native WPF controls for a seamless experience.

### Libraries used:

*   [Magick.NET](https://github.com/dlemstra/Magick.NET) (ImageMagick wrapper for .NET)
