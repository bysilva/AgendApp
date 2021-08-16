const snacktype = {
    SUCCESS: "success",
    ERROR: "error",
    INFO: "info",
    DEFAULT: "default",
}
const snackposition = {
    TOPRIGHT: "top-right",
    BOTTOMCENTER: "bottom-center",
}
function ShowSnackbar(type, position, message) {
    var x = document.getElementById("snackbar");
    x.innerHTML = message;

    switch (type) {
        case "success": x.classList.add("success"); break;
        case "error": x.classList.add("error"); break;
        case "info": x.classList.add("info"); break;
        default: x.classList.add("default"); break;
    }
    switch (position) {
        case snackposition.TOPRIGHT: x.classList.add("top-right"); x.classList.add("showtop"); break;
        case snackposition.BOTTOMCENTER: x.classList.add("bottom-center"); x.classList.add("showbottom"); break;
        default:
    }
    setTimeout(function () { x.className = "" }, 3000);
}