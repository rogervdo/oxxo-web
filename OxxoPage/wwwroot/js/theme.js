document.addEventListener("DOMContentLoaded", function () {
    const toggleButton = document.getElementById("darkModeToggle");
    const body = document.body;

    // Check if dark mode is already enabled (e.g., from localStorage)
    if (localStorage.getItem("darkMode") === "enabled") {
        body.classList.add("dark-mode");
    }

    if (toggleButton) {
        toggleButton.addEventListener("click", function () {
            if (body.classList.contains("dark-mode")) {
                body.classList.remove("dark-mode");
                localStorage.setItem("darkMode", "disabled");
                toggleButton.textContent = "Activate Dark Mode";
            } else {
                body.classList.add("dark-mode");
                localStorage.setItem("darkMode", "enabled");
                toggleButton.textContent = "Activate Light Mode";
            }
        });

        // Set initial button text based on mode
        toggleButton.textContent = body.classList.contains("dark-mode")
            ? "Activate Light Mode"
            : "Activate Dark Mode";
    }
});