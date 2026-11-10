window.followerFiles = {
    downloadPdf: function (fileName, base64Content) {
        const link = document.createElement("a");
        link.href = "data:application/pdf;base64," + base64Content;
        link.download = fileName;
        document.body.appendChild(link);
        link.click();
        link.remove();
    },
    openPdf: async function (fileName, content) {
        const bytes = await content.arrayBuffer();
        const url = URL.createObjectURL(new Blob([bytes], { type: "application/pdf" }));
        const link = document.createElement("a");
        link.href = url;
        link.target = "_blank";
        link.rel = "noopener";
        link.setAttribute("aria-label", "Previsualizar " + fileName);
        document.body.appendChild(link);
        link.click();
        link.remove();
        window.setTimeout(() => URL.revokeObjectURL(url), 60000);
    },
    downloadPdfStream: async function (fileName, content) {
        const bytes = await content.arrayBuffer();
        const url = URL.createObjectURL(new Blob([bytes], { type: "application/pdf" }));
        const link = document.createElement("a");
        link.href = url;
        link.download = fileName;
        document.body.appendChild(link);
        link.click();
        link.remove();
        window.setTimeout(() => URL.revokeObjectURL(url), 60000);
    }
};
