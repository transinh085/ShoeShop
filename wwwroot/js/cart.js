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
						<strong>$${item.price}</strong>
					</td>
					<td>
						<div class="numbers-row">
							<input type="text" value="${item.quantity}" id="quantity_1" class="qty2" name="quantity_1">
							<div class="inc button_inc">+</div><div class="dec button_inc">-</div>
						<div class="inc button_inc">+</div><div class="dec button_inc">-</div></div>
					</td>
					<td>
						<strong>$${item.price * item.quantity}</strong>
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
	//$('#cart-total-price').text(`$${getTotalPrice()}`);
	//$('.cart_bt strong').text(getTotalProduct());
}

$(document).on('click', '.delete-cart-tbl', function () {
	const variantSizeId = $(this).data('id');
	removeCart(variantSizeId);
	loadCart();
	renderCartTable();
});

$(document).on("ajaxComplete", function () {
	renderCartTable();
});
