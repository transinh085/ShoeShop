document.addEventListener("DOMContentLoaded", function () {
  !(function () {
    class e {
      static initValidation() {
        // Add custom validation methods
        $.validator.addMethod(
          "passwordLength",
          function (value) {
            return value.length >= 6;
          },
          "Passwords must be at least 6 characters."
        );

        $.validator.addMethod(
          "passwordNonAlphanumeric",
          function (value) {
            return /[^\w]/.test(value);
          },
          "Passwords must have at least one non-alphanumeric character."
        );

        $.validator.addMethod(
          "passwordLowercase",
          function (value) {
            return /[a-z]/.test(value);
          },
          "Passwords must have at least one lowercase ('a'-'z')."
        );

        $.validator.addMethod(
          "passwordUppercase",
          function (value) {
            return /[A-Z]/.test(value);
          },
          "Passwords must have at least one uppercase ('A'-'Z')."
        );

        // Use custom validation methods in your rules
        Dashmix.helpers("jq-validation");
        jQuery(".js-validation").validate({
          ignore: [],
          rules: {
            "val-fullname": {
              required: true,
            },
            "val-username": {
              required: true,
            },
            "val-email": {
              required: true,
              emailWithDot: true,
            },
            "val-password": {
              required: true,
              passwordLength: true,
              passwordNonAlphanumeric: true,
              passwordLowercase: true,
              passwordUppercase: true,
            },
            "val-birthday": {
              required: true,
            },
            "val-phone": {
              required: true,
            },
          },
          messages: {
            "val-fullname": {
              required: "Please enter a fullname",
            },
            "val-username": {
              required: "Please enter a username",
            },
            "val-email": "Please enter a valid email address",
            "val-password": {
              required: "Please provide a password",
            },
            "val-birthday": {
              required: "Please select a birthday",
            },
            "val-phone": {
              required: "Please enter phone",
            },
          },
        });

        jQuery(".js-validation-edit").validate({
          ignore: [],
          rules: {
            "val-fullname-edit": {
              required: true,
            },
            "val-username-edit": {
              required: true,
            },
            "val-email-edit": {
              required: true,
              emailWithDot: true,
            },
            "val-birthday-edit": {
              required: true,
            },
            "val-phone-edit": {
              required: true,
            },
          },
          messages: {
            "val-fullname-edit": {
              required: "Please enter a fullname",
            },
            "val-username-edit": {
              required: "Please enter a username",
            },
            "val-email-edit": "Please enter a valid email address",
            "val-birthday-edit": {
              required: "Please select a birthday",
            },
            "val-phone-edit": {
              required: "Please enter phone",
            },
          },
        });
      }
      static init() {
        this.initValidation();
      }
    }
    Dashmix.onLoad(() => e.init());
  })();

  // Tạo một hàm để lấy danh sách "allCustomers" từ API
  var customers = null;
  var id = null;
  function getCustomerList(response) {
    customers = response;
    let htmlCus = ``;
      customers.forEach((item, index) => {
      index++;
      let st = item.status
        ? '<span class="badge bg-success">Enable</span>'
        : '<span class="badge bg-danger">Disable</span>';
      htmlCus += `<tr>
                    <td class="d-none d-sm-table-cell text-center">${index}</td>
                    <td class="text-center">
                                                                <img class="img-avatar img-avatar48" src="${
                                                                    !item.profileImageUrl
                                                                    ? "/admin/media/avatars/avatar1.jpg"
                                                                    : item.profileImageUrl
                                                                }" alt="">
                    </td>
                    <td class="d-none d-sm-table-cell">
                                        <a href="javascript:void(0)">${
                                            item.fullName
                                        }</a>
                    </td>
                    <td class="d-none d-sm-table-cell">
                                        ${
                                            item.email
                                        }
                    </td>
                    <td class="d-none d-sm-table-cell">
                                        ${
                                            item.phoneNumber
                                        }
                    </td>
                    <td class="d-none d-sm-table-cell">
                                        ${
                                            item.gender ==
                                            0
                                            ? "Male"
                                            : "Female"
                                        }
                    </td>
                    <td class="d-none d-sm-table-cell">
                                        ${formatDateFromString(
                                            item.birthDay
                                        )}
                    </td>
                    <td class="d-none d-lg-table-cell">
                                        ${st}
                    </td>

                                <td class="text-center col-action">
                        <a class="btn btn-sm btn-alt-secondary user-edit show" href="javascript:void(0)" data-bs-toggle="tooltip" aria-label="Edit" data-bs-original-title="Edit" data-action="edit" data-id="${
                        item.id
                        }">
            <i class="fa fa-fw fa-pencil"></i>
        </a>
                        <a class="btn btn-sm btn-alt-secondary user-delete show" href="javascript:void(0)" data-bs-toggle="tooltip" aria-label="Delete" data-bs-original-title="Delete" data-action="delete" data-id="${
                        item.id
                        }">
            <i class="fa fa-fw fa-times"></i>
        </a>
    </td>
                </tr>`;
    });
    document.getElementById("listCustomer").innerHTML = htmlCus;
  }
  // Gọi hàm để lấy danh sách

  // Gán sự kiện khi modal được ẩn (đóng)
  $("#modal-customer").on("hidden.bs.modal", function () {
    // Xóa các lớp CSS liên quan đến lỗi validation
    $(".is-invalid").removeClass("is-invalid");
    // Tương tự cho các trường khác

    // Xóa thẻ HTML hiển thị lỗi validation
    $(".invalid-feedback").remove();

    $("#val-email").val("");
    $("#val-username").val("");
    $("#val-birthday").val("");
    $("#val-password").val("");
    $("#val-fullname").val("");
    $("#val-phone").val("");
    $("#gender-male").prop("checked", true);
    $("#user_status").prop("checked", true);
  });

  $("#modal-edit-customer").on("hidden.bs.modal", function () {
    $(".is-invalid").removeClass("is-invalid");
    // Xóa thẻ HTML hiển thị lỗi validation
    $(".invalid-feedback").remove();
    $("#val-email-edit").val("");
    $("#val-username-edit").val("");
    $("#val-birthday-edit").val("");
    $("#val-fullname-edit").val("");
    $("#val-phone-edit").val("");
    $("#gender-male-edit").prop("checked", true);
    $("#user_status-edit").prop("checked", true);
  });

  $("#btn-add-user").on("click", function (e) {
    e.preventDefault(); // Ngăn chặn form gửi đi (để xử lý validation trước)

    // Kiểm tra nếu form validation thành công
    if ($(".js-validation").valid()) {
      // Lấy dữ liệu từ các trường input
      var fullname = $("#val-fullname").val();
      var username = $("#val-username").val();
      var email = $("#val-email").val();
      var phone = $("#val-phone").val();
      var birthday = $("#val-birthday").val();
      var password = $("#val-password").val();
      var gender = $("input[name='user_gender']:checked").val();
      var status = $("#user_status").is(":checked");

      // Tạo một đối tượng chứa dữ liệu cần gửi lên máy chủ
      var userData = {
        FullName: fullname,
        UserName: username,
        Email: email,
        PhoneNumber: phone,
        BirthDay: birthday,
        Password: password,
        Gender: gender,
        Status: status,
      };
      $.ajax({
        type: "POST",
        url: "/Admin/Customers/AddCustomer",
        data: userData, // Dữ liệu cần gửi lên máy chủ
        success: function (response) {
          // Xử lý phản hồi từ máy chủ, ví dụ: đóng modal và reset form
          console.log(response);
          // Đóng modal (tuỳ vào mã HTML/CSS của modal)
          $("#modal-customer").modal("hide");
          pagination.getPagination();
          Dashmix.helpers("jq-notify", {
            type: "success",
            icon: "fa fa-check me-1",
            message: "Successfully added customers",
          });
        },
        error: function (xhr, status, error) {
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

  $(document).on("click", "[data-action='edit']", function () {
    id = $(this).data("id");

    let customer = customers.find((c) => c.id === id);
    console.log(customer);
    if (customer) {
      $("#val-fullname-edit").val(customer.fullName);
      $("#val-username-edit").val(customer.userName);
      $("#val-email-edit").val(customer.email);
      $("#val-phone-edit").val(customer.phoneNumber);
      let dateObject = new Date(customer.birthDay);

      // Trích xuất ngày, tháng và năm từ đối tượng Date
      let year = dateObject.getFullYear();
      let month = dateObject.getMonth() + 1; // Tháng bắt đầu từ 0
      let day = dateObject.getDate();
      let formattedDate = year + "-" + month + "-" + day;
      $("#val-birthday-edit").val(formattedDate);
      $("#gender-male-edit").prop("checked", customer.gender === 0);
      $("#gender-female-edit").prop("checked", customer.gender === 1);
      $("#user_status-edit").prop("checked", customer.status);
    }

    $("#modal-edit-customer").modal("show");
  });

  $("#btn-edit-user").on("click", function (e) {
    e.preventDefault();

    if ($(".js-validation-edit").valid()) {
      var fullname = $("#val-fullname-edit").val();
      var username = $("#val-username-edit").val();
      var email = $("#val-email-edit").val();
      var phone = $("#val-phone-edit").val();
      var birthday = $("#val-birthday-edit").val();
      var gender = $("input[name='user_gender']:checked").val();
      var status = $("#user_status").is(":checked");

      var userData = {
        FullName: fullname,
        UserName: username,
        Email: email,
        PhoneNumber: phone,
        Password: "passowrd",
        BirthDay: birthday,
        Gender: gender,
        Status: status,
      };
      $.ajax({
        type: "PUT",
        url: "/Admin/Customers/UpdateCustomer/" + id,
        data: userData,
        success: function (response) {
          $("#modal-edit-customer").modal("hide");
          pagination.getPagination();
          Dashmix.helpers("jq-notify", {
            type: "success",
            icon: "fa fa-check me-1",
            message: "Successfully updated customer",
          });
        },
        error: function (xhr, status, error) {
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

  $(document).on("click", "[data-action='delete']", async function () {
    let trid = $(this).data("id");
    let result = await Swal.fire({
      title: "Are you sure?",
      text: "Would you like to delete this customer?",
      icon: "warning",
      showCancelButton: true,
      confirmButtonText: "Yes, I'm sure",
      cancelButtonText: "Cancel",
    });

    if (result.value) {
      try {
        let response = await $.ajax({
          url: `/Admin/Customers/DeleteCustomer/${trid}`,
          type: "DELETE",
          success: function () {
            Swal.fire(
              "Deleted!",
              "Successfully deleted the customer.",
              "success"
            );
          },
          error: function (xhr, status, error) {
            console.error("Error:", error);
            Swal.fire(
              "Error!",
              "Deletion of the customer was not successful.",
              "error"
            );
          },
        });

        if (response) {
          pagination.getPagination();
        }
      } catch (error) {
        console.error("Error:", error);
        Swal.fire(
          "Error!",
          "Deletion of the customer was not successful.",
          "error"
        );
      }
    }
  });

  const pagination = new Pagination(
    null,
    "/Admin/Customers/allCustomers",
    getCustomerList,
    "searchInput"
  );
});
function formatDateFromString(dateString) {
  const dateObject = new Date(dateString);
  const year = dateObject.getFullYear();
  const month = dateObject.getMonth() + 1;
  const day = dateObject.getDate();
  const formattedDate = `${day}/${month}/${year}`;
  return formattedDate;
}
