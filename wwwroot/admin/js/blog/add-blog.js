let files = [];
let uploadContainer = document.querySelector(".upload-image-container");
let uploadImgInput = document.querySelector(".upload-image-input");
let hasImage = document.querySelector(".has-image");
let noImage = document.querySelector(".no-image");

uploadContainer.addEventListener("click", () => {
    uploadImgInput.click();
});

uploadImgInput.addEventListener("change", (e) => {
    let fileInput = uploadImgInput.files;
    for (let i = 0; i < fileInput.length; i++) {
        if (files.every((item) => item.name != fileInput[i].name)) {
            files.push(fileInput[i]);
        }
    }

    uploadImgInput.value = '';

    showImage();
});

const showImage = () => {
    checkUI();
    let images = "";
    console.info(files);
    files.forEach(function (file, index) {
        images = `<div class="preview-image-item" onClick=(setThumbnail(event))>
                     <img src="${URL.createObjectURL(file)}" alt="image" data-id="${index}" style="margin:0 0; width:600px; height:400px"/>
                        <span class="btn-delete-image" onClick="handleDelete(event,${index}, this.parentElement)"><i class="fa fa-fw fa-times"></i></span></div>`;
    });
    hasImage.innerHTML = images;
};


const checkUI = () => {
    if (files.length > 0) {
        noImage.style.display = "none";
    } else {
        noImage.style.display = "block";
    }
};
const handleDelete = (event, index, parentElement) => {
    event.stopPropagation();
    files.splice(index, 1);
    checkUI();
    parentElement.parentNode.removeChild(parentElement);
};

const setThumbnail = (event) => {
    event.stopPropagation();
    let clName = event.target;
    document.querySelector(".preview-image-item img.active")?.classList.remove('active');
    clName.classList.add('active');
}
