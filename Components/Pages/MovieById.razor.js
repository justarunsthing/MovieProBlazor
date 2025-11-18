let src = "";
let modal = null;

function createEmbed() {
    document.getElementById("movie-trailer").setAttribute("src", src);
}

function destroyEmbed() {
    document.getElementById("movie-trailer").setAttribute("src", "");
}

export function initVideoPlayer(videoUrl) {
    if (modal) {
        modal.removeEventListener("shown.bs.modal", createEmbed);
        modal.removeEventListener("hidden.bs.modal", destroyEmbed);
    }

    modal = document.getElementById("trailer-modal");
    src = videoUrl;

    modal.addEventListener("shown.bs.modal", createEmbed);
    modal.addEventListener("hidden.bs.modal", destroyEmbed);
}