# Lost Object Adventure

Game **Hidden Object** (tìm đồ vật bị thất lạc) được làm bằng **Unity**.

> Scene chính: `Assets/Scenes/game.unity`  
> Unity Editor: `6000.4.0f1` (Unity 6)

## Gameplay

- Nhiệm vụ: tìm các đồ vật bị ẩn trong bản đồ (click/chạm để “nhặt”).
- Có **NPC/Quest**: hoàn thành mục tiêu để nhận thưởng coin.
- Có **Shop/Items**:
  - **Magnifying Glass**: chỉ định (specify) 1 vật cần tìm.
  - **Highlight**: highlight các vật cần tìm trong vài giây.
  - Coin / mua vật phẩm / xem ads nhận thưởng (tuỳ cấu hình).

## Điều khiển

- PC (Editor): **Click** vào đồ vật để thu thập.
- Mobile: **Tap** vào đồ vật để thu thập.

## Chạy dự án (Development)

### Yêu cầu

- **Unity 6** `6000.4.0f1` (khuyến nghị đúng phiên bản để tránh lỗi import).

### Mở và chạy trong Unity

1. Mở project bằng Unity Hub.
2. Mở scene: `Assets/Scenes/game.unity`
3. Nhấn **Play**.

### Build

Trong Unity: **File → Build Settings…**  
Chọn platform (Android/Windows/macOS/…) → **Build**.

## Cấu trúc thư mục

- `Assets/Scenes/`: scene của game (scene chính: `game.unity`)
- `Assets/Lost-Object-Adventure/`: asset + script gameplay
- `Assets/Carrot-Framework-App/`, `Assets/Carrot-Ads/`: framework/ads (tuỳ cấu hình dự án)
- `ProjectSettings/`, `Packages/`: cấu hình Unity

## Lưu ý Git

Unity thường **không** commit các thư mục sau (chỉ nên để local):

- `Library/`
- `Temp/`
- `Logs/`

(`.gitignore` đã có sẵn trong repo, nhưng nếu các thư mục trên đã lỡ bị track thì cần untrack trước khi push.)

## License

MIT — xem file `Assets/Lost-Object-Adventure/LICENSE`.

## Credits

- Tác giả: Trần Thiện Thanh

