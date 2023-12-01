var icerik = "";

function KartEkle() {

    icerik += `<div class="card">
                <div class="card-header d-flex">
                    <h5 class="card-title d-flex mx-5">Tarih: </h5>
                    <input type="text" class="form-control-sm" aria-label="Small">
                </div>
                <div class="card-body">
                    <div class="d-flex my-3">
                        <p class="card-text mt-3 mx-5">Kilo: </p>
                        <input type="text" class="form-control-sm" aria-label="Small">
                    </div>
                    <div class="d-flex my-3">
                        <p class="card-text mt-3 mx-5">Boy: </p>
                        <input type="text" class="form-control-sm" aria-label="Small">
                    </div>
                    <div class="d-flex my-3">
                        <p class="card-text mt-3 mx-5">Yağ Oranı:</p>
                        <input type="text" class="form-control-sm" aria-label="Small">
                    </div>
                    <div class="d-flex my-3">
                        <p class="card-text mt-3 mx-5">Kas Kütlesi:</p>
                        <input type="text" class="form-control-sm" aria-label="Small">
                    </div>
                    <div class="d-flex my-3">
                        <p class="card-text mt-3 mx-5">Vücut Kitle İndeksi:</p>
                        <input type="text" class="form-control-sm" aria-label="Small">
                    </div>
                    
                </div>
            </div>`;
    $("#yeniKartlar").html(icerik);

}