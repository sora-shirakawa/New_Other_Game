# Wave Roguelike 実装プラン（第1弾 + Editor自動セットアップ）

## このコミットで追加した内容
- プレイ中のゲーム状態を管理する `GameManager`
- プレイヤー移動・基本ステータス・被弾処理
- 自動攻撃（最寄り敵を探索して弾を発射）
- 敵の追尾・接触ダメージ・死亡処理
- ダメージインターフェース `IDamageable`
- Editorメニューから最低限の検証シーンを生成する `NewOtherGamePrototypeBuilder`
- シーン開始時に自動でRun開始する `AutoStartRun`

## Editorスクリプトによる最速セットアップ
1. Unityでプロジェクトを開く
2. メニュー `Tools > New Other Game > Build Prototype Scene` を実行
3. 自動生成されるもの
   - `GameManager` + `AutoStartRun`
   - `Player`（移動 / 自動攻撃）
   - `Enemy` 2体
   - `Assets/Prefabs/Projectile.prefab`
   - `Assets/Data/Enemy_Default.asset`
   - `Enemy` タグ / レイヤー（未作成なら追加）
4. Play して、WASD移動・自動攻撃・敵追尾を確認

## 手動セットアップ（必要な場合のみ）
1. `GameManager` を空オブジェクトにアタッチ
2. `Player` プレハブに以下をアタッチ
   - `PlayerController`
   - `PlayerStats`
   - `AutoAttackController`
   - Collider2D + Rigidbody2D(kinematic)
3. `Projectile` プレハブに
   - `Projectile`
   - Collider2D(isTrigger)
4. `Enemy` プレハブに
   - `EnemyController`
   - Collider2D + Rigidbody2D(dynamic)
5. 敵には `Enemy` タグを付与

## 次の実装予定
- Experience Orb とレベルアップ
- スキル3択UI
- WaveManager / SpawnManager
- Stage遷移とボス戦
