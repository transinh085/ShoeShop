let cartData = [];

const getTotalProduct = () => {
	let totalProduct = getCartStorage().reduce(function (sum, currentItem) {
		return sum + currentItem.quantity;
	}, 0);

	return totalProduct;
}

const getTotalPrice = () => {
	let totalPrice = cartData.reduce(function (sum, currentItem) {
		return sum + ((currentItem.priceSale != 0 ? currentItem.priceSale : currentItem.price) * currentItem.quantity);
	}, 0);

	return totalPrice;
}

const getCartStorage = () => {
	return localStorage.getItem('cart') ? JSON.parse(localStorage.getItem('cart')) : [];
}

const mergeCartData = (productItem) => {
	const result = productItem.map((item) => {
		const quantity = getCartStorage().find(cartItem => cartItem.variantSizeId == item.variantSizeId).quantity;
		return {
			...item,
			quantity
		}
	})
	return result;
}

const loadCart = () => {
	const arrVariantSizeId = getCartStorage().map(item => item.variantSizeId);
	$.ajax({
		url: '/Product/GetCart',
		type: 'POST',
		contentType: 'application/json',
		data: JSON.stringify({ carts: arrVariantSizeId }),
		success: function (response) {
			cartData = mergeCartData(response);
			console.info(cartData);
			renderOrderSummary();
		},
		error: function (error) {
			console.error('Error:', error);
		}
	});
}

loadCart();

const renderOrderSummary = () => {
	let html = '';
	cartData.forEach(item => {
		html += `<tr><td class="ps-0"><div class="d-flex gap-3 align-items-center">
                   <img src="/img/products/${item.thumbnail}" alt="product" style="height: 70px" />
                 <div>
                 <a class="fs-sm fw-semibold" href="/product/detail/${item.productId}">${item.quantity} x ${item.title}</a>
                 <div class="fs-sm text-muted">Size: ${item.sizeName}, Color: ${item.colorName}</div></div></div></td>
                 <td class="pe-0 fw-medium text-end">
				 <span>$${item.priceSale != 0 ? item.priceSale : item.price}</span>
				 </td>
                 </tr>`
	})
	$("#tbl-product").html(html);
	renderPrice();
}

const renderPrice = () => {
	let tt = parseFloat(getTotalPrice());
	var shippingCost = parseFloat($('input[name="checkout-delivery"]:checked').val());
	$("#sub-total").html(`$${tt}`);
	$("#shipping-fee").html(`$${shippingCost}`);
	$("#total").html(`$${tt + shippingCost}`);
}

$('input[name="checkout-delivery"]').change(function () {
	renderPrice();
});

$("#btn-complete").on('click', function (e) {
	const paymentMethod = $('input[name="checkout-payment"]:checked').val();

	const checkoutData = {
		Cart: getCartStorage(),
		ShippingMethodId: $('input[name="checkout-delivery"]:checked').data('id'),
		PaymentMethodId: paymentMethod,
		AddressId: $('#choose-address').val(),
		NewAddress: {
			FullName: $("#checkout-fullname").val(),
			Email: $("#checkout-email").val(),
			Phone: $("#checkout-phone").val(),
			SpecificAddress: $("#checkout-address").val(),
		},
		OrderDescription: $("#checkout-desc").val()
	}

	console.info(checkoutData);

	$.ajax({
		url: '/payment/CreatePaymentUrl',
		type: 'POST',
		contentType: 'application/json',
		data: JSON.stringify(checkoutData),
		beforeSend: function () {
			console.log('Request is pending...');
		},
		success: function (response) {
			if (paymentMethod == 1) {
				location.href = response;
			}
		},
		error: function (error) {
			console.error('Error:', error);
		}
	});
});

const checkChooseAddress = () => {
	let chooseAddress = $('#choose-address').val();
	console.info(chooseAddress);
	var addressItem = $('.address-item[data-id="' + chooseAddress + '"]');
	addressItem.removeClass('d-none');
}

checkChooseAddress();

$('#choose-address').on('change', function () {
	$('.address-item').addClass('d-none');
	checkChooseAddress();
});