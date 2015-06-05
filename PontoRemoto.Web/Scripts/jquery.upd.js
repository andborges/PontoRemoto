(function($) {
    $.fn.upd = function(model) {
	    debug("Plugin selection count: " + this.length);

		return this.each(function() {
			var container = $(this);
			
			bindText(container, model);
			bindHtml(container, model);

			bindShow(container, model);
			bindShowChildren(container, model);
			bindHide(container, model);
			bindHideChildren(container, model);
			bindClass(container, model);
			bindClassChildren(container, model);
		});
    };

	function bindText(container, model) {
		var binds = container.find("[upd-text]");
		debug("Text: " + binds.length);

		binds.each(function() {
			var bind = $(this);
			var prop = bind.attr("upd-text");

			bind.text(model[prop]);
		});
	}

	function bindHtml(container, model) {
		var binds = container.find("[upd-html]");
		debug("Html: " + binds.length);

		binds.each(function() {
			var bind = $(this);
			var prop = bind.attr("upd-html");

			bind.html(model[prop]);
		});
	}

	function bindShow(bind, model) {
		if (bind.attr("upd-show")) {
			var func = createBooleanFunction(bind.attr("upd-show"));
			debug(func(model));
			
			bind.toggle(func(model));
		}
	}

	function bindHide(bind, model) {
		if (bind.attr("upd-hide")) {
			var func = createBooleanFunction(bind.attr("upd-hide"));
			debug(func(model));

			bind.toggle(!func(model));
		}
	}

	function bindClass(bind, model) {
		if (bind.attr("upd-class")) {
			var attr = bind.attr("upd-class").split(":");
			
			var cssClass = attr[0].trim();
			debug(cssClass);
			
			var func = createBooleanFunction(attr[1]);
			debug(func(model));
			
			bind.toggleClass(cssClass, func(model));
		}
	}

	function bindShowChildren(container, model) {
		var binds = container.find("[upd-show]");
		debug("Show: " + binds.length);
		
		binds.each(function() {
			bindShow($(this), model);
		});
	}

	function bindHideChildren(container, model) {
		var binds = container.find("[upd-hide]");
		debug("Hide: " + binds.length);
		
		binds.each(function() {
			bindHide($(this), model);
		});
	}

	function bindClassChildren(container, model) {
		var binds = container.find("[upd-class]");
		debug("Class: " + binds.length);
		
		binds.each(function() {
			bindClass($(this), model);
		});
	}

	function createBooleanFunction(stringExpression) {
		var expression = stringExpression.trim().replace(/\$/g, "m.");
		debug(expression);
			
		return new Function("m", "return " + expression);
	}
	
	function debug(message) {
        if (window.console && window.console.log) {
            // window.console.log(message);
        }
    };
})(jQuery);