// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


function convertToPdf() {
    var fileInput = document.getElementById("htmlFileInput");
    var file = fileInput.files[0];

    if (file) {
        var formData = new FormData();
        formData.append("file", file);

        fetch("/Home/ConvertToPdf", {
            method: "POST",
            body: formData
        })
            .then(response => {
                if (response.ok) {
                    response.blob().then(blob => {
                        const url = URL.createObjectURL(blob);

                        // Display the PDF file in the iframe
                        var pdfViewer = document.getElementById("pdfViewer");
                        pdfViewer.src = url;

                        // Enable the download button
                        var downloadButton = document.getElementById("downloadButton");
                        downloadButton.disabled = false;
                    });
                }
            })
            .catch(error => {
                console.log("Error:", error);
            });
    }
}

function clearFile() {
    var fileInput = document.getElementById("htmlFileInput");
    var pdfViewer = document.getElementById("pdfViewer");
    var downloadButton = document.getElementById("downloadButton");

    // Clear the file input value
    fileInput.value = null;

    // Clear the PDF viewer by setting the iframe source to an empty string
    pdfViewer.src = "";

    // Disable the download button
    downloadButton.disabled = true;
}

function downloadPdf() {
    var pdfViewer = document.getElementById("pdfViewer");
    var pdfUrl = pdfViewer.src;

    // Create a temporary link for downloading
    var link = document.createElement("a");
    link.href = pdfUrl;
    link.download = "output.pdf";

    // Trigger the download
    link.click();
}
