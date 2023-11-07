﻿let productId = $("#product-id").val();
let variants = [];

$(document).ready(async function () {
    try {
        const response = await fetch(`/Admin/Products/GetVariant/${productId}`);
        if (!response.ok) {
            throw new Error('Network response was not ok.');
        }
        const data = await response.json();

        variants = data;
        const variantPromises = variants.map(async variant => {
            const index = variant.images.findIndex(image => image == variant.thumbnail);
            const imagesFiles = await convertImagesToFiles(variant.images);

            return {
                ...variant,
                images: imagesFiles,
                thumbnail: index
            };
        });

        variants = await Promise.all(variantPromises);
        showVariant();
        showVariantSize();
    } catch (error) {
        console.error(error);
    }
});

const fetchImageFile = async (imageUrl) => {
    try {
        const response = await fetch(imageUrl);
        if (response.ok) {
            const blob = await response.blob();
            return new File([blob], "filename.jpg", { type: blob.type });
        } else {
            throw new Error('Network response was not ok.');
        }
    } catch (error) {
        throw error;
    }
};

const convertImagesToFiles = async (images) => {
    const filePromises = images.map(async imageUrl => fetchImageFile(`/img/products/${imageUrl}`));
    const files = await Promise.all(filePromises);
    return files;
};



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
  files.forEach(function (file, index) {
      images += `<div class="preview-image-item" onClick=(setThumbnail(event))>
                     <img src="${URL.createObjectURL(file)}" alt="image" data-id="${index}"/>
                        <span class="btn-delete-image" onClick="handleDelete(event,${index}, this.parentElement)"><i class="fa fa-fw fa-times"></i></span></div>`;
  });
  hasImage.innerHTML = images;
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

const checkUI = () => {
  if (files.length > 0) {
    noImage.style.display = "none";
  } else {
    noImage.style.display = "block";
  }
};

// Handle change variant
let colorEl = $("#color-id");
let sizeEl = $("#size-id");

$("#btn-add-variant").click(() => {
    if ($("#form-variant").valid()) {
        const variant = getObjVariant();
        variants.push(variant);
        showVariant();
        resetDialogVariant();
        showVariantSize();
        $("#modal-variant").modal("hide");
    }
})

const resetDialogVariant = () => { 
    files = [];
    showImage(files);
    colorEl.val([]).trigger('change');
    sizeEl.val([]).trigger('change');
    $("#form-variant").validate().resetForm();
    $("#form-variant").find(".is-invalid").removeClass("is-invalid");
}

const showVariant = () => {
    console.info(variants);
    const htmlArray = variants.map((variant, index) => {
        const { colorName, sizes, thumbnail } = variant;
        const textSize = sizes.map(item => item.sizeName).join(", ");
        const imageUrl = URL.createObjectURL(variant.images[thumbnail ?? 0]);
        return `<tr><th>
            <img class="img-variant" src="${imageUrl}" alt="" />
        </th>
        <td>${colorName}</td>
        <td>${textSize}</td>
        <td class="text-center">
            <div class="btn-group">
                <button data-index="${index}" class="btn-up-variant btn btn-sm btn-alt-secondary" ${index == 0 ? "disabled" : ''}>
                    <i class="fa fa-arrow-up"></i>
                </button>
                <button data-index="${index}" class="btn-down-variant btn btn-sm btn-alt-secondary" ${index == variants.length - 1 ? "disabled" : ''}>
                    <i class="fa fa-arrow-down"></i>
                </button>
                <button data-index="${index}" class="btn-edit-variant btn btn-sm btn-alt-secondary">
                    <i class="fa fa-pencil-alt"></i>
                </button>
                <button data-index="${index}" class="btn-delete-variant btn btn-sm btn-alt-secondary">
                    <i class="fa fa-times"></i>
                </button>
            </div>
        </td></tr>`;
    });

    const newHtml = htmlArray.join('');
    const variantColorContainer = $("#variant-color");
    if (newHtml !== variantColorContainer.html()) {
        variantColorContainer.html(newHtml);
    }

    if (variants.length > 0) {
        $(".variants-container").show();
    } else {
        $(".variants-container").hide();
    }
};

$(".variants-container").on("click", ".btn-edit-variant", function (event) {
    const index = $(this).data("index");
    $("#btn-add-variant").hide();
    $("#btn-update-variant").show();
    $("#btn-update-variant").data("index", index);
    showModalVariant(variants[index]);
    $("#modal-variant").modal('show');
});

$(".variants-container").on("click", ".btn-delete-variant", function (event) {
    const index = $(this).data("index");
    variants.splice(index, 1);
    showVariant();
    showVariantSize();
});

$(".variants-container").on("click", ".btn-up-variant", function (event) {
    const index = $(this).data("index");
    if (index > 0 && index < variants.length) {
        [variants[index], variants[index - 1]] = [variants[index - 1], variants[index]];
        showVariant();
    }
});

$(".variants-container").on("click", ".btn-down-variant", function (event) {
    const index = $(this).data("index");
    if (index >= 0 && index < variants.length - 1) {
        [variants[index], variants[index + 1]] = [variants[index + 1], variants[index]];
        showVariant();
    }
});


$("#btn-save-product").click(() => {
    const productData = {
        Name: $('#product-name').val(),
        Price: $('#product-price').val(),
        Description: CKEDITOR.instances['js-ckeditor'].getData(),
        Status: $('#product-status').prop("checked"),
        Slug: $('#product-slug').val(),
        Category: $('#category-id').val(),
        Brand: $('#brand-id').val(),
        Variants: variants 
    };

    console.info(productData);

    var formData = new FormData();

    // Thêm các thuộc tính của sản phẩm vào FormData
    formData.append('Name', productData.Name);
    formData.append('Price', productData.Price);
    formData.append('Description', productData.Description);
    formData.append('Status', productData.Status);
    formData.append('Slug', productData.Slug);
    formData.append('Category', productData.Category);
    formData.append('Brand', productData.Brand);

    // Thêm các biến thể của sản phẩm
    productData.Variants.forEach((variant, variantIndex) => {
        formData.append(`Variants[${variantIndex}].ColorId`, variant.colorId);
        formData.append(`Variants[${variantIndex}].ColorName`, variant.colorName);

        // Thêm các hình ảnh blob cho biến thể hiện tại
        variant.images.forEach((imageBlob, imageIndex) => {
            formData.append(`Variants[${variantIndex}].Images`, imageBlob, imageBlob.name);
        });

        // Thêm các kích thước cho biến thể hiện tại
        variant.sizes.forEach((size, sizeIndex) => {
            formData.append(`Variants[${variantIndex}].Sizes[${sizeIndex}].SizeId`, size.sizeId);
            formData.append(`Variants[${variantIndex}].Sizes[${sizeIndex}].Stock`, size.stock);
            formData.append(`Variants[${variantIndex}].Sizes[${sizeIndex}].Active`, size.active);
        });
    });

    console.log(formData);

    $.ajax({
        type: 'POST',
        url: '/Admin/Products/Create',
        data: formData,
        processData: false,
        contentType: false,
        success: function (response) {
            console.log(response);
        },
        error: function (error) {
            console.error(error);
        }
    });
})

const showModalVariant = (variant) => {
    const { colorId, sizes, images, thumbnail } = variant;
    const sizeArr = sizes.map(item => item.sizeId)
    files = images;
    showImage();
    $(`.preview-image-item img[data-id='${thumbnail}']`).addClass('active');
    colorEl.val(colorId).trigger('change');
    sizeEl.val(sizeArr).trigger('change');
}

$("#btn-update-variant").click(function () {
    const index = $(this).data("index");
    variants[index] = getObjVariant();
    showVariant();
    showVariantSize();
    $("#btn-add-variant").show();
    $("#btn-update-variant").hide();
    resetDialogVariant();
});

const getObjVariant = () => {
    const colorId = colorEl.val();
    const colorName = $("#color-id option:selected").text();
    const sizes = sizeEl.val().map(sizeId => {
        const sizeName = $(`#size-id option[value='${sizeId}']`).text();

        console.info(`.input-stock[data-size='${sizeId}'][data-color='${colorId}']`);
        console.info($(`.input-stock[data-size='${sizeId}'][data-color='${colorId}']`).val());

        const stock = $(`.input-stock[data-size='${sizeId}'][data-color='${colorId}']`).val() ?? '';
        return { sizeId, sizeName, stock, active: true };
    });
    const images = files;
    const thumbnail = $(".preview-image-item img.active").data("id");
    const variant = { colorId, colorName, sizes, images, thumbnail };
    return variant;
}

const showVariantSize = () => {
    let html = '';
    if (variants.length > 0) {
        $(".table-variant-container").show();
    } else {
        $(".table-variant-container").hide();
    }
    variants.forEach(variant => {
        variant.sizes.forEach(function (item, index) {
            const { sizeId, sizeName, stock, active } = item;
            html += `<tr class="row-size" data-id="${sizeId}" data-name="${sizeName}">
            <th scope="row">${++index}</th>
            <td>${variant.colorName + ' / ' + sizeName}</td>
            <td>
                <input data-size="${sizeId}" data-color="${variant.colorId}" type="number" class="form-control form-control-sm w-50 input-stock" value="${stock}">
            </td>
            <td class="form-check form-switch mb-0">
                <div class="form-check form-switch">
                    <input data-size="${sizeId}" data-color="${variant.colorId}" class="form-check-input input-active" type="checkbox" checked="${active}">
                </div>
            </td></tr>`;
        });
    })

    $("#table-variant").html(html);
};

$(document).on('input', '.input-stock', function () {
    const sizeId = $(this).data('size');
    const colorId = $(this).data('color');
    const newStockValue = $(this).val();

    const variant = variants.find(item => item.colorId == colorId);
    let size = variant.sizes.find(item => item.sizeId == sizeId);
    size.stock = newStockValue;
});

$(document).on('input', '.input-active', function () {
    const sizeId = $(this).data('size');
    const colorId = $(this).data('color');
    const newStockValue = $(this).prop("checked");

    const variant = variants.find(item => item.colorId == colorId);
    let size = variant.sizes.find(item => item.sizeId == sizeId);
    size.active = newStockValue;
});

// Validate
Dashmix.onLoad((() => class {
    static initValidation() {
        Dashmix.helpers("jq-validation"),
        jQuery.validator.addMethod("checkFiles", function (value, element) {
            return files.length > 0;
         }, "Please upload at least one file");

        jQuery("#form-variant").validate({
            ignore: [],
            rules: {
                "color-id": {
                    required: !0
                },
                "size-id": {
                    required: !0
                },
                "upload-image": {
                    checkFiles: true
                }
            },
            messages: {
                "color-id": "Please select a value!",
                "size-id": "Please select values!",
                "upload-image": "Please upload at least one file !"
            }
        });

        jQuery("#product-form").validate({
            ignore: [],
            rules: {
                "product-name": {
                    required: !0
                },
                "product-price": {
                    required: !0
                },
                "product-slug": {
                    required: !0
                },
                "brand-id": {
                    required: !0
                },
                "category-id": {
                    required: !0
                }
            },
            messages: {
                "product-name": "Please enter a name!",
                "product-price": "Please enter a price!",
                "product-slug": "Please enter a slug!",
                "brand-id": "Please select a value!",
                "category-id": "Please select values!",
            }
        });

        jQuery(".js-select2").on("change", (e => {
            jQuery(e.currentTarget).valid()
        }));

        jQuery("#upload-image").on("change", (e => {
            jQuery(e.currentTarget).valid()
        }));
    }

    static init() {
        this.initValidation()
    }
}.init()));