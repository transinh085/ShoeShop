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

    $(this).val(null);

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

colorEl.on("change.select2", function (e) {
  if ($(this).val()) {
    sizeEl.prop("disabled", false);
  }
});

sizeEl.on("change.select2", function (e) {
    showVariantSize(sizeEl.val());
});

const showVariantSize = (sizes) => {
  let html = "";
  sizes.forEach(function (item, index) {
      let sizeName = $(`#size-id option[value='${item}']`).text();
      html += `<tr class="row-size" data-id="${item}" data-name="${sizeName}">
    <th scope="row">${++index}</th>
    <td>${sizeName}</td>
    <td>
        <input type="number" class="form-control form-control-sm w-50 input-stock">
    </td>
    <td class="form-check form-switch mb-0">
        <div class="form-check form-switch">
            <input class="form-check-input input-active" type="checkbox" checked>
        </div>
    </td></tr>`;
  });
    if (sizes.length > 0) {
        $(".table-variant-container").show();
    } else { 
        $(".table-variant-container").hide();
    }
  $("#table-variant").html(html);
};

$("#btn-add-variant").click(() => {
    const variant = getObjVariant();
    variants.push(variant);
    showVariant();
    resetDialogVariant();
})

const getSize = () => {
    let sizesRow = $(".row-size");
    let result = [];

    sizesRow.each(function () {
        const ob = {
            id: $(this).data("id"),
            name: $(this).data("name"),
            stock: $(this).find(".input-stock").val(),
            active: $(this).find(".input-active").prop("checked")
        };
        result.push(ob);
    });

    return result;
}

const resetDialogVariant = () => { 
    files = [];
    showImage(files);
    colorEl.val([]).trigger('change');
    sizeEl.val([]).trigger('change');
}

const showVariant = () => {
    const htmlArray = variants.map((variant, index) => {
        const { colorName, sizes, thumbnail } = variant;
        const textSize = sizes.map(item => item.name).join(", ");
        const imageUrl = URL.createObjectURL(variant.images[thumbnail ?? 0]);
        return `<tr><th>
            <img class="img-variant" src="${imageUrl}" alt="" />
        </th>
        <td>${colorName}</td>
        <td>${textSize}</td>
        <td class="text-center">
            <div class="btn-group">
                <button data-index="${index}" type="button" class="btn-up-variant btn btn-sm btn-alt-secondary">
                    <i class="fa fa-arrow-up"></i>
                </button>
                <button data-index="${index}" type="button" class="btn-down-variant btn btn-sm btn-alt-secondary">
                    <i class="fa fa-arrow-down"></i>
                </button>
                <button data-index="${index}" type="button" class="btn-edit-variant btn btn-sm btn-alt-secondary">
                    <i class="fa fa-pencil-alt"></i>
                </button>
                <button data-index="${index}" type="button" class="btn-delete-variant btn btn-sm btn-alt-secondary">
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
    showVariant()
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

        console.info(variant.images);

        // Thêm các hình ảnh blob cho biến thể hiện tại
        variant.images.forEach((imageBlob, imageIndex) => {
            formData.append(`Variants[${variantIndex}].Images`, imageBlob, imageBlob.name);
        });

        // Thêm các kích thước cho biến thể hiện tại
        variant.sizes.forEach((size, sizeIndex) => {
            formData.append(`Variants[${variantIndex}].Sizes[${sizeIndex}].Id`, size.id);
            formData.append(`Variants[${variantIndex}].Sizes[${sizeIndex}].Name`, size.name);
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
    const arrSize = sizes.map(item => item.id);
    files = images;
    showImage();
    $(`.preview-image-item img[data-id='${thumbnail}']`).addClass('active');
    colorEl.val(colorId).trigger('change');
    sizeEl.val(arrSize).trigger('change');
    setStockAndActive(sizes);
}

const setStockAndActive = (sizes) => {
    let sizesRow = $(".row-size");

    sizesRow.each(function (index) {
        const { stock, active } = sizes[index];
        $(this).find(".input-stock").val(stock);
        $(this).find(".input-active").prop("checked", active);
    });
}

$("#btn-update-variant").click(() => {
    const index = $(this).data("index");
    const variant = getObjVariant();
    variants[index] = variant;
    showVariant();
    resetDialogVariant();
    $("#btn-add-variant").show();
    $("#btn-update-variant").hide();
})

const getObjVariant = () => {
    const variant = {
        colorId: colorEl.val(),
        colorName: $("#color-id option:selected").text(),
        sizes: getSize(),
        images: files,
        thumbnail: $(".preview-image-item img.active").data("id")
    };
    console.info(variant);
    return variant;
}