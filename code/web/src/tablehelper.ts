import { CameraData } from "./interfaces";


export function AddCameraRowToTable(tableid: string, camera: CameraData) {
    const table = document.getElementById(tableid) as HTMLTableElement;
    if (table == undefined)
        return;

    // assume table always contains at least one tbody
    const body = table.getElementsByTagName('TBODY')[0];

    // prepare element.
    const row = document.createElement('TR') as HTMLTableRowElement;
    AddCellToRow(row, camera.number.toString());
    AddCellToRow(row, camera.camera.toString());
    AddCellToRow(row, camera.latitude.toString());
    AddCellToRow(row, camera.longitude.toString());

    // apend row to table body.
    body.appendChild(row);
}

function AddCellToRow(row: HTMLTableRowElement, celldata: string): HTMLTableCellElement {
    const cell = document.createElement('TD') as HTMLTableCellElement;
    cell.innerHTML = celldata;
    row.appendChild(cell);
    return cell;
}

export function GetTableidForCameranumber(cameranumber: number) {
    const isDivisibleByThree = cameranumber % 3 == 0;
    const isDivisibleByFive = cameranumber % 5 == 0;

    if (isDivisibleByFive && isDivisibleByThree) {
        return 'column15'
    } else if (isDivisibleByFive) {
        return 'column5'
    } else if (isDivisibleByThree) {
        return 'column3';
    } else {
        return 'columnOther';
    }
}