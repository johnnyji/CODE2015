$(document).ready(function() {
	var scrollHeight = $(window).height(); - 50;

	setTimeout(function() {
		$(".result").addClass("animated bounceInLeft");
		setTimeout(function() {
			$(".result").removeClass("animated bounceInLeft");
		}, 1000);
	}, 500);

	$(window).scroll(function() {
		if (scrollHeight) {
			$(".fa-chevron-down").fadeOut(400);
		}
	});
});