const renderCartTable = () => {
	let html = '';
	if (cartData.length > 0) {
		cartData.forEach(item => {
			html += `<tr>
					<td>
						<div class="thumb_cart">
							<img src="/img/products/${item.thumbnail}" alt="Image">
						</div>
						<span class="item_cart">${item.title}</span>
					</td>
					<td>
						<strong>${item.sizeName} / ${item.colorName}</strong>
					</td>
					<td>
						${item.priceSale != 0 ? `<strong class="text-danger">$${item.priceSale}</strong><br>
						<span class="text-decoration-line-through text-secondary">$${item.price}</span>`
						: `<strong class="text-danger">$${item.price}</strong>`}
					</td>
					<td>
						<div class="numbers-row">
							<input type="text" data-id="${item.variantSizeId}" value="${item.quantity}" id="quantity_1" class="qty2" name="quantity_1">
							<div class="inc button_inc">+</div><div class="dec button_inc">-</div>
					</td>
					<td class="text-center">
						<strong>$${(item.priceSale != 0 ? item.priceSale : item.price) * item.quantity}</strong>
					</td>
					<td class="options">
						<a href="#" class="delete-cart-tbl" data-id="${item.variantSizeId}"><i class="ti-trash"></i></a>
					</td>
				</tr>`
		})
	} else {
		html = `<tr><td colspan="6"><div class="d-flex justify-content-center align-items-center gap-3 flex-column py-4"><i class="ti ti-shopping-cart" style="font-size: 50px"></i><p class="mb-0">There are currently no products</p></div></tr></td>`
	}
	$('#tbl-cart').html(html);
	$('#total-cart-price-tbl').text(`$${getTotalPrice()}`);
	//$('.cart_bt strong').text(getTotalProduct());
}

$(document).on('click', '.button_inc', function () {
	var iEl = $(this).parent().find("input");
	const variantSizeId = $(iEl).data('id');
	handleChangeQuantity(variantSizeId, parseInt(iEl.val()));
	renderCartTable();
});


$(document).on('click', '.delete-cart-tbl', async function () {
	let result = await Swal.fire({
		title: "Are you sure?",
		text: "Would you like to delete this product?",
		icon: "warning",
		showCancelButton: true,
		confirmButtonText: "Yes, I'm sure",
		cancelButtonText: "Cannel"
	});

	if (result.value) {
		try {
			const variantSizeId = $(this).data('id');
			removeCart(variantSizeId);
			renderCartTable();
		} catch (error) {
			console.error("Error:", error);
			Swal.fire("Lỗi !", "Deletion of the product was not successful.", "error");
		}
	}
});

$(document).on("ajaxComplete", function () {
	renderCartTable();
});
