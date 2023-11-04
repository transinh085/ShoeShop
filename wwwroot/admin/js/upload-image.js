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
    console.info(fileInput);
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
    showVariantSize($("#color-id option:selected").text(), sizeEl.val());
});

const showVariantSize = (color, sizes) => {
  let html = "";
  sizes.forEach(function (item, index) {
      let sizeName = $(`#size-id option[value='${item}']`).text();
      html += `<tr class="row-size" data-id="${item}" data-name="${sizeName}">
    <th scope="row">${index++}</th>
    <td>${color + " / " + sizeName}</td>
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
    const variant = {
        color_id: colorEl.val(),
        color_name: $("#color-id option:selected").text(),
        sizes: getSize(),
        images: files,
        thumbnail: $(".preview-image-item img.active").data("id")
    };
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
    showImage();
    colorEl.val([]).trigger('change');
    sizeEl.val([]).trigger('change');
}

const showVariant = () => {
    let html = '';
    variants.forEach((variant, index) => {
        const { color_id, color_name, images, sizes, thumbnail } = variant;
        const textSize = sizes.map(item => item.name).join(", ");
        html += `<tr><th>
          <img class="img-variant" src="${URL.createObjectURL(images[thumbnail ?? 0])}" alt="" />
        </th>
         <td>${color_name}</td>
         <td>${textSize}</td>
         <td class="text-center">
         <div class="btn-group">
             <button data-index="${index}" type="button" class="btn-edit-variant btn btn-sm btn-alt-secondary" data-bs-toggle="tooltip" title="Edit">
                    <i class="fa fa-pencil-alt"></i>
             </button>
             <button data-index="${index}" type="button" class="btn-delete-variant btn btn-sm btn-alt-secondary" data-bs-toggle="tooltip" title="Delete">
                    <i class="fa fa-times"></i>
             </button>
         </div>
        </td></tr>`;
    })
    $("#variant-color").html(html);
}

$(document).on("click", ".btn-delete-variant", function () {
    let index = $(this).data("index");
    variants.splice(index, 1);
    showVariant()
});