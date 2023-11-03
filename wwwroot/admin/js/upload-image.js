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
  showVariant($("#color-id option:selected").text(), sizeEl.val());
});

const showVariant = (color, sizes) => {
  let html = "";
  sizes.forEach(function (item, index) {
      html += `<tr class="row-size" data-id="${item}">
    <th scope="row">${index++}</th>
    <td>${color + " / " + item}</td>
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
        color: colorEl.val(),
        sizes: getSize(),
        images: files,
        thumbnail: $(".preview-image-item img.active").data("id")
    };
    variants.push(variant);
    console.info(variants);
    //resetDialogVariant();
})

const getSize = () => {
    let sizesRow = $(".row-size");
    let result = [];

    sizesRow.each(function () {
        const ob = {
            id: $(this).data("id"),
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