// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function TurnOff() {
    let info_box = document.getElementById("info_box");

    if (info_box.style.display === "none") {
        info_box.style.display = "block";
    }
    else {
        info_box.style.display = "none";
    }
}