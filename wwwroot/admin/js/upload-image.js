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

    for (let i = 0; i < fileInput.length; i++) {
        if (files.every((item) => item.name != fileInput[i].name)) {
            files.push(fileInput[i]);
        }
    }

    showImage();
});

const showImage = () => {
    checkUI();
    let images = '';
    files.forEach(function (file, index) {
        images += `<div class="preview-image-item">
                     <img src="${URL.createObjectURL(file)}" alt="image" />
                        <span class="btn-delete-image" onClick="handleDelete(event,${index}, this.parentElement)"><i class="fa fa-fw fa-times"></i></span></div>`
    });
    hasImage.innerHTML = images;
}

const handleDelete = (event, index, parentElement) => {
    event.stopPropagation();
    files.splice(index, 1);
    checkUI();
    parentElement.parentNode.removeChild(parentElement);
}

const checkUI = () => {
    if (files.length > 0) {
        noImage.style.display = "none";
    } else {
        noImage.style.display = "block";
    }
}

// Handle change variant 
let colorEl = $("#color-id");
let sizeEl = $("#size-id");

colorEl.on("change.select2", function (e) {
    console.info("colorEl change");
});

sizeEl.on("change.select2", function (e) {
    console.info("sizeEl change");
});