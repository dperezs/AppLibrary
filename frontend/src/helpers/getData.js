export const getData = async (url, header = {}) => {
    try {
        const res = await fetch(url, header);
        const json = res.json();
        return json;
    } catch (error) {
        console.error(error);
        return {};
    }
}

export const getFormData = (data) => {
    var form_data = new FormData();

    for (var key in data) {
        if (data[key])
            form_data.append(key, data[key]);
    }
    return form_data;
}