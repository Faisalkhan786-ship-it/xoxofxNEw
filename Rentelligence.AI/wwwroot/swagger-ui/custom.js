setTimeout(() => {

    const button = document.createElement("button");

    button.innerHTML = "🌙";
    button.className = "theme-toggle-btn";

    document.body.appendChild(button);

    button.addEventListener("click", function () {

        document.body.classList.toggle("dark-mode");

        if (document.body.classList.contains("dark-mode")) {
            button.innerHTML = "☀️";
        } else {
            button.innerHTML = "🌙";
        }
    });

}, 1000);