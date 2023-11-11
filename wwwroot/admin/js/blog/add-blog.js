let files = [];
let uploadContainer = document.querySelector(".upload-image-container");
let uploadImgInput = document.querySelector(".upload-image-input");
let hasImage = document.querySelector(".has-image");
let noImage = document.querySelector(".no-image");

let url = "";
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
    url = URL.createObjectURL(file);
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


//$("#btn-save-blog").click(() => {
//    const blogData = {
//        Name: $('#blog-name').val(),
//        Slug: $('#blog-slug').val(),
//        CreateBy: "Admin",
//        CreateAt: new Date().getTime,
//        Topic: $('#topic-id').val(),
//        Content: CKEDITOR.instances['js-ckeditor'].getData(),
//        IsDelete: false
//    };

//    console.info(blogData);

//    $.ajax({
//        type: 'POST',
//        url: '/Admin/Blogs/Create',
//        data: formData,
//        processData: false,
//        contentType: false,
//        success: function (response) {
//            console.log(response);
//        },
//        error: function (error) {
//            console.error(error);
//        }
//    });
//})


$("#btn-add-blog").click(() => {

    // Kiểm tra nếu form validation thành công
    if ($(".js-validation").valid()) {
        // Lấy dữ liệu từ các trường input
        var name = $("#blog-name").val();
        var slug = $("#blog-slug").val();
        var thumb = $url;
        var topic = $("#topic-id").value;
        var content = CKEDITOR.instances['js-ckeditor'].getData();
        var gender = $("input[name='user_gender']:checked").val();
        var isDel = false;


        alert(thumb + topic);

        // Tạo một đối tượng chứa dữ liệu cần gửi lên máy chủ
        var blogData = {
            Name: name,
            Slug: slug,
            Thumbnail: thumb,
            TopicID: topic,
            Content: content,
            IsDelete: isDel,
        };
        $.ajax({
            type: "POST",
            url: "/Admin/Blogs/Create",
            data: userData, // Dữ liệu cần gửi lên máy chủ
            success: function (response) {
                // Xử lý phản hồi từ máy chủ, ví dụ: đóng modal và reset form
                console.log(response);
                
                pagination.getPagination();
                Dashmix.helpers("jq-notify", {
                    type: "success",
                    icon: "fa fa-check me-1",
                    message: "Successfully added customers",
                });
            },
            error: function (xhr, status, error) {
                // Xử lý lỗi nếu có
                console.log("Conflict");
                console.log(xhr.responseJSON.error);
                console.log(error);
                console.log(status);
                if (xhr?.responseJSON?.error)
                    Dashmix.helpers("jq-notify", {
                        type: "warning",
                        icon: "fa fa-exclamation me-1",
                        message: xhr.responseJSON.error,
                    });
            },
        });
    }
});