

// Lắng nghe sự kiện thay đổi của input file
document.querySelector('.thumbnail').addEventListener('change', function (event) {
        // Lấy đối tượng input file
    var input = event.target;
        // Đảm bảo người dùng đã chọn ít nhất một file
        if (input.files && input.files[0]) {
            // Đọc dữ liệu của file hình ảnh
            var reader = new FileReader();
            // Xác định sự kiện xảy ra sau khi đọc xong file
            reader.onload = function (e) {
                // Hiển thị hình ảnh trong thẻ <img>
                document.querySelector('.imagePreview').src = e.target.result;
                document.querySelector('.imagePreview').style.display = 'block'; // Hiển thị thẻ <img>
            };
            alert(e.target.result)

            // Đọc file hình ảnh dưới dạng URL dữ liệu (data URL)
            reader.readAsDataURL(input.files[0]);
        }
    });

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