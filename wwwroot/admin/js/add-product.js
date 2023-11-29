let variants = [];

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
        images += `<div class="preview-image-item" draggable="true" onClick=(setThumbnail(event)) ondragstart="handleDragStart(event, ${index})" ondragover="handleDragOver(event)" ondrop="handleDrop(event)">
                     <img src="${URL.createObjectURL(file)}" alt="image" data-id="${index}"/>
                     <span class="btn-delete-image" onClick="handleDelete(event, ${index}, this.parentElement)"><i class="fa fa-fw fa-times"></i></span>
               </div>`;
    });
    hasImage.innerHTML = images;
};

let draggedIndex = null;

const handleDragStart = (event, index) => {
    draggedIndex = index;
    event.dataTransfer.effectAllowed = 'move';
    event.dataTransfer.setData('text/html', event.target.innerHTML);
};

const handleDragOver = (event) => {
    event.preventDefault();
};

const handleDrop = (event) => {
    event.preventDefault();
    const dropIndex = parseInt(event.target.getAttribute('data-id'), 10);
    if (!isNaN(dropIndex) && draggedIndex !== null) {
        // Swap the positions in the files array
        const temp = files[dropIndex];
        files[dropIndex] = files[draggedIndex];
        files[draggedIndex] = temp;

        // Redraw the images
        showImage();
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
    $(`#color-id option[value='${variants[index].colorId}']`).prop("disabled", false);
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
    if ($("#product-form").valid()) {
        if (variants.length != 0) {
            const productData = {
                Name: $('#product-name').val(),
                Price: $('#product-price').val(),
                PriceSale: $('#product-price-sale').val(),
                Description: CKEDITOR.instances['js-ckeditor'].getData(),
                IsFeatured: $('#is-featured').prop("checked"),
                Status: $('#product-status').val() == 1,
                Slug: $('#product-slug').val(),
                Label: $('#product-label').val(),
                Category: $('#category-id').val(),
                Brand: $('#brand-id').val(),
                Variants: variants
            };

            console.info(productData);

            var formData = new FormData();

            // Thêm các thuộc tính của sản phẩm vào FormData
            formData.append('Name', productData.Name);
            formData.append('Price', productData.Price);
            formData.append('PriceSale', productData.PriceSale);
            formData.append('Description', productData.Description);
            formData.append('Status', productData.Status);
            formData.append('IsFeatured', productData.IsFeatured);
            formData.append('Slug', productData.Slug);
            formData.append('Label', productData.Label);
            formData.append('Category', productData.Category);
            formData.append('Brand', productData.Brand);

            // Thêm các biến thể của sản phẩm
            productData.Variants.forEach((variant, variantIndex) => {
                formData.append(`Variants[${variantIndex}].ColorId`, variant.colorId);
                formData.append(`Variants[${variantIndex}].Thumbnail`, variant.thumbnail);

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
                    Dashmix.helpers('jq-notify', { type: 'success', icon: 'fa fa-check me-1', message: 'Create product successfully !' });
                    setTimeout(() => {
                        location.href = "/admin/products";
                    }, 2000);
                },
                error: function (error) {
                    console.error(error);
                }
            });
        } else {
            Dashmix.helpers('jq-notify', { type: 'danger', icon: 'fa fa-times me-1', message: 'Variants empty !' });
        }
    }
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
            </tr>`;
        });
    })

        //< td class="form-check form-switch mb-0" >
        //    <div class="form-check form-switch">
        //        <input data-size="${sizeId}" data-color="${variant.colorId}" class="form-check-input input-active" type="checkbox" checked="${active}">
        //    </div>
        //    </td >

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

        $.validator.addMethod("checkSlug",
            function (value, element) {
                var result = false;
                $.ajax({
                    type: "POST",
                    async: false,
                    url: "/admin/products/checkslug",
                    data: { slug: $("#product-slug").val() },
                    success: function (data) {
                        result = data.isUnique;
                    }
                });
                return result;
            },
            "This slug is already in use !"
        );

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
                    required: !0,
                    number: !0
                },
                "product-slug": {
                    required: !0,
                    checkSlug: !0
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
                "product-slug": {
                    required: "Please enter a slug!",
                    checkSlug: "This slug is already in use"
                },
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

const slugify = (str) => {
    return String(str)
        .normalize('NFKD')
        .replace(/[\u0300-\u036f]/g, '')
        .trim()
        .toLowerCase()
        .replace(/[^a-z0-9 -]/g, '')
        .replace(/\s+/g, '-')
        .replace(/-+/g, '-');
}

$("#product-name").blur(function () {
    const name = $(this).val();
    $("#product-slug").val(slugify(name));
});

$('#modal-variant').on('shown.bs.modal', function () {
    $('#size-id').select2({ closeOnSelect: false, dropdownParent: $('#modal-variant') });
    disabledSelectColor();
});

const disabledSelectColor = () => {
    $(`#color-id option`).prop("disabled", false);
    variants.forEach((item) => {
        $(`#color-id option[value='${item.colorId}']`).prop("disabled", true);
    })
}