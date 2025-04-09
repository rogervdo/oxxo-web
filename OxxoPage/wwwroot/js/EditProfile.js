//funcionalidad abrir y cerrar primer pop up ------------
const openSelect = document.getElementById("image");
const closeBtn = document.getElementById("closeImgSelect-btn");
const imgSelect = document.getElementById("img-Select");
const boxImg = document.getElementById("box-img-select");

openSelect.addEventListener("click", () => {
  boxImg.classList.add("show");
  imgSelect.classList.add("show");
});

closeBtn.addEventListener("click", () => {
  boxImg.classList.remove("show");
  imgSelect.classList.remove("show");
});

// funcionalidad pop up 2: grid de iamagenes ---------------
const chooseImg = document.getElementById("choose-img");
const gridBox = document.getElementById("box-img-grid");
const gridPopup = document.getElementById("popUpGrid");
const closeGridBtn = document.getElementById("closeGridBtn");

// Abrir pop-up 2 desde botón "Elegir imagen"
chooseImg.addEventListener("click", () => {
  gridBox.classList.add("show");
  gridPopup.classList.add("show");
});

// Cerrar pop-up 2
closeGridBtn.addEventListener("click", () => {
  gridBox.classList.remove("show");
  gridPopup.classList.remove("show");
});

// funcionalidad de cerrar pop up 1 y 2 desde el grid de imagenes ---------------
const saveBtn = document.getElementById("saveGridSelection");
const boxGrid = document.getElementById("box-img-grid");
const popUpGrid = document.getElementById("popUpGrid");

const boxSelect = document.getElementById("box-img-select");
const popUpSelect = document.getElementById("img-Select");

saveBtn.addEventListener("click", () => {
  // Cierra el segundo pop-up
  boxGrid.classList.remove("show");
  popUpGrid.classList.remove("show");

  // También cierra el primero
  boxSelect.classList.remove("show");
  popUpSelect.classList.remove("show");
});

//funcionalidad upload image
