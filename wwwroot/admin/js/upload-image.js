let files = [];
let uploadContainer = document.querySelector(".upload-image-container");
let uploadImgInput = document.querySelector(".upload-image-input");
let hasImage = document.querySelector('.has-image');
let noImage = document.querySelector('.no-image');

uploadContainer.addEventListener("click", () => {
    uploadImgInput.click(); 
}) 

uploadImgInput.addEventListener("change", (e) => {
    let fileInput = uploadImgInput.files;
    console.info(fileInput);

    for (let i = 0; i < fileInput.length; i++) {
        files.push(fileInput[i]);
    }

    showImage();
});

const showImage = () => {
    let images = '';
    if (files.length > 0) {
        noImage.style.display = "none";
    } else {
        noImage.style.display = "block";
    }
    files.forEach(function (file, index) {
        images += `<div class="preview-image-item">
                     <img src="${URL.createObjectURL(file)}" alt="image" />
                        <span class="btn-delete-image" onClick="handleDelete(event,${index})"><i class="fa fa-fw fa-times"></i></span></div>`
    });
    hasImage.innerHTML = images;
}

const handleDelete = (event, index) => {
    event.stopPropagation();
    files.splice(index, 1);
    showImage();
}