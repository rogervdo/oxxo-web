const boxImg = document.getElementById("box-img");
const openSelect = document.getElementById("image");
const closeBtn = document.getElementById("closeImgSelect-btn");
const imgSelect = document.getElementById("img-Select");

openSelect.addEventListener("click", () => {
  boxImg.classList.add("show");
  imgSelect.classList.add("show");
});

closeBtn.addEventListener("click", () => {
  boxImg.classList.remove("show");
  imgSelect.classList.remove("show");
});
