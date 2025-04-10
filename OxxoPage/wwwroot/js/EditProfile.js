// Elementos generales del popup 1 (elegir selec img predeterminada)
const openSelect = document.getElementById("image"); // La imagen de perfil
const closeBtn = document.getElementById("closeImgSelect-btn"); // Botón de cierre
const imgSelect = document.getElementById("img-Select");
const boxImg = document.getElementById("box-img-select");

// Elementos de la imagen
let profilePic = document.getElementById("profileImage");
// Variable para guardar el estado original antes de hacer cambios
let originalImageSrc = profilePic.src;
const gridImages = document.querySelectorAll(".grid-image"); // Imágenes del grid

// Funcionalidad abrir y cerrar primer pop-up
openSelect.addEventListener("click", () => {
  originalImageSrc = profilePic.src; // Guardar imagen original
  boxImg.classList.add("show");
  imgSelect.classList.add("show");
});

closeBtn.addEventListener("click", () => {
  boxImg.classList.remove("show");
  imgSelect.classList.remove("show");
});

// Elementos del popup 2 (grid de fotos a escoger) ---------
const chooseImg = document.getElementById("choose-img"); // Cuadro en popup 1
const gridBox = document.getElementById("box-img-grid");
const gridPopup = document.getElementById("popUpGrid");
const closeGridBtn = document.getElementById("closeGridBtn"); // x

// Funcionalidad abrir pop-up 2 desde "Elegir imagen"
chooseImg.addEventListener("click", () => {
  gridBox.classList.add("show");
  gridPopup.classList.add("show");
});

// Cerrar popup 2
closeGridBtn.addEventListener("click", () => {
  gridBox.classList.remove("show");
  gridPopup.classList.remove("show");
});

// Al seleccionar imagen del grid: actualizar imagen y volver a popup 1
gridImages.forEach((img) => {
  img.addEventListener("click", () => {
    //actualiza profile img
    profilePic.src = img.src;
    //se quita pop 2
    gridBox.classList.remove("show");
    gridPopup.classList.remove("show");
    //se muestra pop1
    boxImg.classList.add("show");
    imgSelect.classList.add("show");
  });
});

// Botones guardar y cancelar
const saveBtn = document.getElementById("saveGridSelection");
const cancelBtn = document.getElementById("cancelGridSelection");

// Función cerrar con botones guardar o cancelar
function closePopups() {
  boxImg.classList.remove("show");
  imgSelect.classList.remove("show");
}

// Guardar : cerrar los popups
saveBtn.addEventListener("click", closePopups);

// Cancelar: regresa imagen original (anterior en src) y cerrar todo
cancelBtn.addEventListener("click", () => {
  profilePic.src = originalImageSrc; // Volver a imagen original
  closePopups();
});

//Funcionalidad de guardar valor seleccionado en var y enviar a post
// Seleccionar la imagen y actualizar el hidden form
function selectImage(imageName) {
  // Asignar el nombre de la imagen seleccionada al campo oculto
  document.getElementById("selectedImage").value = imageName;

  // Cerrar el popup 2 y volver al popup 1
  document.getElementById("box-img-grid").classList.remove("show");
  document.getElementById("popUpGrid").classList.remove("show");
  document.getElementById("box-img-select").classList.add("show");
  document.getElementById("img-Select").classList.add("show");
}
