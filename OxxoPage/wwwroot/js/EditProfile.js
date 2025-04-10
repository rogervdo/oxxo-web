// Elementos generales del popup 1 (seleccionar imagen)
const openSelect = document.getElementById("image");
const closeBtn = document.getElementById("closeImgSelect-btn");
const imgSelect = document.getElementById("img-Select");
const boxImg = document.getElementById("box-img-select");

// Elementos del popup 2 (grid)
const chooseImg = document.getElementById("choose-img");
const gridBox = document.getElementById("box-img-grid");
const gridPopup = document.getElementById("popUpGrid");
const closeGridBtn = document.getElementById("closeGridBtn");

// Botones
const saveBtn = document.getElementById("saveGridSelection");
const cancelBtn = document.getElementById("cancelGridSelection");

// Elementos de imagen
let profilePic = document.getElementById("profileImage");
let inputFile = document.getElementById("input-file");
const gridImages = document.querySelectorAll(".grid-image");

// Variable para guardar el estado original antes de hacer cambios
let originalImageSrc = profilePic.src;

//funcionalidad abrir y cerrar primer pop up -----------------
// Abrir popup 1: guardar imagen original antes de cambios
openSelect.addEventListener("click", () => {
  originalImageSrc = profilePic.src;
  boxImg.classList.add("show");
  imgSelect.classList.add("show");
});

// Cerrar popup 1
closeBtn.addEventListener("click", () => {
  boxImg.classList.remove("show");
  imgSelect.classList.remove("show");
});

// Abrir pop-up 2 desde botón "Elegir imagen" ------------
chooseImg.addEventListener("click", () => {
  gridBox.classList.add("show");
  gridPopup.classList.add("show");
});

// Cerrar popup 2
closeGridBtn.addEventListener("click", () => {
  gridBox.classList.remove("show");
  gridPopup.classList.remove("show");
});

// Al subir imagen: actualizar ft perfil
inputFile.onchange = function () {
  if (inputFile.files[0]) {
    profilePic.src = URL.createObjectURL(inputFile.files[0]);
  }
};

// Al seleccionar imagen del grid: actualizar inmediatamente y volver a popup 1
gridImages.forEach((img) => {
  img.addEventListener("click", () => {
    profilePic.src = img.src;
    gridBox.classList.remove("show");
    gridPopup.classList.remove("show");
    boxImg.classList.add("show");
    imgSelect.classList.add("show");
  });
});

// Guardar cambios: simplemente cerrar los popups
saveBtn.addEventListener("click", () => {
  boxGrid.classList.remove("show");
  popUpGrid.classList.remove("show");
  boxImg.classList.remove("show");
  imgSelect.classList.remove("show");
});

// Cancelar cambios: restaurar la imagen original y cerrar todo
cancelBtn.addEventListener("click", () => {
  profilePic.src = originalImageSrc; // Volver a imagen original
  inputFile.value = ""; // Limpiar input file si se usó
  boxGrid.classList.remove("show");
  popUpGrid.classList.remove("show");
  boxImg.classList.remove("show");
  imgSelect.classList.remove("show");
});

// funcionalidad de cerrar pop up 1 y 2  ------------------------
const boxGrid = document.getElementById("box-img-grid");
const popUpGrid = document.getElementById("popUpGrid");

const boxSelect = document.getElementById("box-img-select");
const popUpSelect = document.getElementById("img-Select");

//cierra al guardar
saveBtn.addEventListener("click", () => {
  // Cierra el segundo pop-up
  boxGrid.classList.remove("show");
  popUpGrid.classList.remove("show");

  // También cierra el primero
  boxSelect.classList.remove("show");
  popUpSelect.classList.remove("show");
});

//cierra al cancelar
cancelBtn.addEventListener("click", () => {
  // Cierra el segundo pop-up
  boxGrid.classList.remove("show");
  popUpGrid.classList.remove("show");

  // También cierra el primero
  boxSelect.classList.remove("show");
  popUpSelect.classList.remove("show");
});
