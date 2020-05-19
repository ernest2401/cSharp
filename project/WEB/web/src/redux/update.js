import update from 'immutability-helper';

update.extend('$popById', (value, object) => {
	const obj = JSON.parse(JSON.stringify(object));

	for (var i = 0; i < obj.length; i++) {
		if (obj[i] && obj[i].id === value) {
			obj.splice(i, 1);
			break;
		}
	}
	return obj;
});


update.extend('$auto', (value, object) => ((object)
	? update(object, value)
	: update({}, value))
);

//Extension method that can set value by path
update.set = (obj, path, value) => {
	const pathMap = {};
	let pathPointer = pathMap;

	const pathArray = path.split('.');

	pathArray.forEach((item, idx) => {
		if (idx === pathArray.length - 1) {
			pathPointer['$auto'] = { [item]: { $set: value } };
		}
		else {
			const temp = {};
			pathPointer['$auto'] = { [item]: temp };
			pathPointer = temp;
		}
	});
	return update(obj, pathMap);
};

//Extension method that can unset value by path
update.unset = (obj, path) => {
	const pathMap = {};
	let pathPointer = pathMap;

	const pathArray = path.split('.');

	pathArray.forEach((item, idx) => {
		if (idx === pathArray.length - 1) {
			pathPointer['$auto'] = { $unset: [`${item}`] };
		}
		else {
			const temp = {};
			pathPointer['$auto'] = { [item]: temp };
			pathPointer = temp;
		}
	});

	return update(obj, pathMap);
}

export default update;